using adsmap.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using adsmap.EntityFrameworkModel;
using Microsoft.Extensions.DependencyInjection;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace adsmap.DataAccessLayer
{
    public class GlobalTypeRepository
    {
        public List<UnitItemData> GetUnitItemsByTerminal(UnitItemFilter unitItemFilter)
        {
            IList<int> terminalIdes = unitItemFilter.TerminalIdes;
            DateTime limitDate = unitItemFilter.LimitDate;
            bool isFirstRequest = unitItemFilter.IsFirstRequest;
            //string terminalId = unitItemFilter.TerminalId;
            int componentId = unitItemFilter.ComponentId;
            int roleId = unitItemFilter.RoleId;
            double minValue = unitItemFilter.MinValue;
            double maxValue = unitItemFilter.MaxValue;
            //DateTime dateTime = unitItemFilter.DateTime;
            bool isSubFilter = unitItemFilter.IsSubFilter;
            double southWestLat = unitItemFilter.SouthWest.Lat;
            double southWestLng = unitItemFilter.SouthWest.Lng;
            double northEastLat = unitItemFilter.NorthEast.Lat;
            double northEastLng = unitItemFilter.NorthEast.Lng;

            var context = Program.host.Services.CreateScope().ServiceProvider.GetRequiredService<ScadaDbContext>();
            using (context)
            {

                var terminalComponents = from tc in context.TerminalComponents
                                         from terId in terminalIdes
                                         where terId == tc.TerminalId && tc.ActiveModeId != null && (componentId == -1 || componentId == tc.ComponentId)
                                         select new { tc.Id, tc.ActiveModeId };

                var filteredTerminalComponents = from activeMode in context.ActiveModes
                                                 from tc in terminalComponents
                                                 where tc.ActiveModeId == activeMode.Id && activeMode.isForDisplay == true && (roleId == -1 || roleId == activeMode.RoleId)
                                                 select new { tc.Id, tc.ActiveModeId, activeMode.AlertValue };


                var activities = from activity in context.Activities
                                 from tc in filteredTerminalComponents
                                 where tc.Id == activity.TerminalComponentId && (isFirstRequest || activity.Date <= limitDate) && (!isSubFilter || (activity.LastValue >= minValue && activity.LastValue <= maxValue))
                                 select new { activity.TerminalItemId, tc.ActiveModeId, activity.LastValue, tc.AlertValue };

                var unitItemValueList = from terminalItem in context.TerminalItems
                                        from activity in activities
                                        where activity.TerminalItemId == terminalItem.Id && terminalItem.UnitItemId != null
                                        select new { terminalItem.UnitItemId, activity.ActiveModeId, activity.LastValue, activity.AlertValue };

                var unitItemList = from unitItem in context.UnitItems
                                   from unitItemValue in unitItemValueList
                                   where unitItem.Id == unitItemValue.UnitItemId && //unitItem.MotherUnitItemId == null
                                   unitItem.Latitude <= northEastLat && unitItem.Latitude >= southWestLat &&
                                       unitItem.Longtitude <= northEastLng && unitItem.Longtitude >= southWestLng
                                   select new { unitItem.Id, unitItemValue.ActiveModeId, unitItemValue.LastValue, unitItemValue.AlertValue, unitItem.Latitude, unitItem.Longtitude };

                List<UnitItemData> itemLocationList = new List<UnitItemData>();
                foreach (var item in unitItemList)
                {
                    UnitItemData unitItem = new UnitItemData(item.Id, (int)item.ActiveModeId, item.LastValue, (double)item.AlertValue, item.Latitude, item.Longtitude);
                    itemLocationList.Add(unitItem);
                }
                return itemLocationList;
            }
        }

        public List<UnitItemDetails> GetUnitItemDetails(int unitItemId)
        {
            var context = Program.host.Services.CreateScope().ServiceProvider.GetRequiredService<ScadaDbContext>();
            using (context)
            {
                IQueryable<int> terminalItemsIdes = from terminalItem in context.TerminalItems
                                                    where terminalItem.UnitItemId == unitItemId
                                                    select terminalItem.Id;

                var activities = from activity in context.Activities
                                 where terminalItemsIdes.Contains(activity.TerminalItemId)
                                 select new { activity.TerminalComponentId, activity.LastValue, activity.Date };

                var terminalComponent = from tc in context.TerminalComponents
                                        join activity in activities on tc.Id equals activity.TerminalComponentId
                                        select new { tc.ComponentId, tc.ActiveModeId, activity.LastValue, activity.Date };


                var components = from component in context.Components
                                 join tc in terminalComponent on component.Id equals tc.ComponentId
                                 select new { component.ComponentTypeId, tc.ActiveModeId, component.ModelNo, tc.LastValue, tc.Date };


                var componentNames = from componentType in context.ComponentTypes
                                     join component in components on componentType.Id equals component.ComponentTypeId
                                     select new { component.ActiveModeId, componentType.Name, component.ModelNo, component.LastValue, component.Date };

                var itemDetails = from activeMode in context.ActiveModes
                                  join componentName in componentNames on activeMode.Id equals componentName.ActiveModeId
                                  select new { activeMode.RoleId, componentName.Name, componentName.ModelNo, componentName.LastValue, activeMode.AlertValue, componentName.Date };

                var itemDetailsFinal = from role in context.Roles
                                       join itemDetail in itemDetails on role.Id equals itemDetail.RoleId
                                       select new { itemDetail.Name, itemDetail.ModelNo, roleName = role.Name, itemDetail.LastValue, itemDetail.AlertValue, itemDetail.Date };

                List<UnitItemDetails> itemDetailsList = new List<UnitItemDetails>();

                foreach (var item in itemDetailsFinal)
                {
                    itemDetailsList.Add(new UnitItemDetails(item.Name, item.ModelNo, item.roleName, item.LastValue, item.AlertValue, item.Date));
                }

                return itemDetailsList;
            }
        }

        public IList<FilterComponent> GetFilterComponents(string accountId)
        {
            var context = Program.host.Services.CreateScope().ServiceProvider.GetRequiredService<ScadaDbContext>();
            using (context)
            {
                //*****//
                //**1**//
                //*****//
                IQueryable<int> terminalIdes = from accountTerminal in context.AccountTerminals
                                               where accountId == accountTerminal.AccountId
                                               select accountTerminal.TerminalId;
                var terminals = from terminal in context.Terminals
                                where terminalIdes.Contains(terminal.Id) //&& terminal.IsStandard == false
                                select new { terminal.Id, terminal.Name };

                IList<FilterComponent> filterComponents = new List<FilterComponent>();

                foreach (var terminal in terminals)
                {
                    FilterComponent newItem = new FilterComponent(terminal.Id, terminal.Name);
                    filterComponents.Add(newItem);

                    //*****//
                    //**2**//
                    //*****//
                    IQueryable<int> componentIdes = from tc in context.TerminalComponents
                                                    where tc.TerminalId == terminal.Id && tc.ActiveModeId != null
                                                    select tc.ComponentId;
                    var components = from component in context.Components
                                     where componentIdes.Contains(component.Id)
                                     select new { component.Id, component.ModelNo };

                    //IQueryable<int> componentTypeIdes = from component in context.Components
                    //                                    where componentIdes.Contains(component.Id)
                    //                                    select component.ComponentTypeId;
                    //var componentTypes = from componentType in context.ComponentTypes
                    //                     where componentTypeIdes.Contains(componentType.Id) //&& terminal.IsStandard == false
                    //                     select new { componentType.Id, componentType.Name };

                    newItem.SubFilterComponents = new List<FilterComponent>();

                    foreach (var component in components)
                    {
                        FilterComponent newSubItem = new FilterComponent(component.Id, component.ModelNo);
                        newItem.SubFilterComponents.Add(newSubItem);

                        //*****//
                        //**3**//
                        //*****//
                        IQueryable<int?> activeModeIdes = from tc in context.TerminalComponents
                                                          where tc.TerminalId == terminal.Id && tc.ActiveModeId != null && tc.ComponentId == component.Id
                                                          select tc.ActiveModeId;
                        IQueryable<int> roleIdes = from activeMode in context.ActiveModes
                                                   where activeModeIdes.Contains(activeMode.Id)
                                                   select activeMode.RoleId;
                        var roles = from role in context.Roles
                                    where roleIdes.Contains(role.Id) //&& terminal.IsStandard == false
                                    select new { role.Id, role.Name };

                        newSubItem.SubFilterComponents = new List<FilterComponent>();
                        foreach (var role in roles)
                        {
                            newSubItem.SubFilterComponents.Add(new FilterComponent(role.Id, role.Name));
                        }
                    }

                }
                return filterComponents;
            }


        }

        public IList<TerminalType> GetTerminals(string accountId)
        {
            var context = Program.host.Services.CreateScope().ServiceProvider.GetRequiredService<ScadaDbContext>();
            using (context)
            {
                IQueryable<int> terminalIdes = from accountTerminal in context.AccountTerminals
                                               where accountId == accountTerminal.AccountId
                                               select accountTerminal.TerminalId;
                var terminals = from terminal in context.Terminals
                                where terminalIdes.Contains(terminal.Id) //&& terminal.IsStandard == false
                                select new { terminal.Id, terminal.Name };

                IList<TerminalType> terminalTypes = new List<TerminalType>();

                foreach (var item in terminals)
                {
                    terminalTypes.Add(new TerminalType(item.Id, item.Name));
                }
                return terminalTypes;
            }
        }

        public async Task<bool> UpdateCenter(string id,double lat, double lng)
        {
            var context = Program.host.Services.CreateScope().ServiceProvider.GetRequiredService<ScadaDbContext>();
            using (context)
            {
                IQueryable<Account> queryableAccount = from acc in context.Accounts
                                                       where (acc.Id == id)
                                                       //&& (acc.LastSavedLatitude == lat1)
                                                       //&& (acc.LastSavedLongitude == lng1)
                                                       select acc;
                if (queryableAccount == null)
                {
                    return false;
                }
                else
                {
                    Account account = (Account)queryableAccount.First();
                    account.LastSavedLatitude = lat;
                    account.LastSavedLongitude = lng;

                    context.Entry(account).State = EntityState.Modified;
                    await context.SaveChangesAsync();

                    return  true;
                }
            }
        }

        public async Task<LocationType> GetUserMapCenterAsync(string accountId)
        {
            var context = Program.host.Services.CreateScope().ServiceProvider.GetRequiredService<ScadaDbContext>();
            using (context)
            {
                Account account = await context.Accounts.AsNoTracking().FirstOrDefaultAsync(acc => acc.Id == accountId);

                if (account == null)
                {
                    return null;
                }
                else
                {
                    return new LocationType(account.LastSavedLatitude, account.LastSavedLongitude);
                }
            }
        }

        public LocationType GetUserMapCenter(string accountId)
        {
            var context = Program.host.Services.CreateScope().ServiceProvider.GetRequiredService<ScadaDbContext>();
            using (context)
            {
                Account account = context.Accounts.AsNoTracking().FirstOrDefault(acc => acc.Id == accountId);

                if (account == null)
                {
                    return null;
                }
                else
                {
                    return new LocationType(account.LastSavedLatitude, account.LastSavedLongitude);
                }
            }
        }

    }
}
