using adsmap.DataAccessLayer;
using adsmap.EntityFrameworkModel;
using adsmap.Models;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Http;

namespace adsmap.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UnitItemsController : ControllerBase
    {
        private readonly GlobalTypeRepository globalTypeRepository = new GlobalTypeRepository();

        // POST: api/UnitItems
        [HttpPost]
        public ActionResult<List<UnitItemData>> PostToGetUnitItemsData(UnitItemFilter unitItemFilter)
        {
            //********************************************* Filter parameters Logic *********************************************//

            string accountId = unitItemFilter.Id;
            unitItemFilter.TerminalIdes = globalTypeRepository.GetTerminals(accountId).Select(ter => ter.Id).ToList();

            unitItemFilter.IsFirstRequest = (unitItemFilter.DateTime == DateTime.Parse("1 / 31 / 2000 10:00:00 PM")) ? true : false;

            if (unitItemFilter.DateTime.Equals(DateTime.Now))
            {
                TimeSpan oneMinute = new TimeSpan(0, 1, 0);
                unitItemFilter.LimitDate = DateTime.Now.Subtract(oneMinute);
            }
            else
            {
                unitItemFilter.LimitDate = unitItemFilter.DateTime;
            }

            IList<UnitItemData> unitItemList = globalTypeRepository.GetUnitItemsByTerminal(unitItemFilter);

            //********************************************* Average or Formula Logic *********************************************//

            List<UnitItemData> unitItemListFinal = new List<UnitItemData>();

            var positions = (from ui in unitItemList
                             select new { ui.Location.Lat, Lng = ui.Location.Lng }).ToList();

            for (int i = 0; i < unitItemList.Count; i++)
            {
                UnitItemData unitItem = unitItemList[i];
                bool isOverFlow = false;
                int count = positions.Count(pos => unitItem.Location.Lat == pos.Lat && unitItem.Location.Lng == pos.Lng);
                if (count > 1)
                {
                    IList<UnitItemData> commonPosSubList = unitItemList.Where(ui => ui.Location.Lat == unitItem.Location.Lat && ui.Location.Lng == unitItem.Location.Lng).ToList();

                    IList<int> activeModeIdes = (from ui in commonPosSubList
                                                 select ui.ActiveModeId).ToList();

                    for (int j = 0; j < commonPosSubList.Count; j++)
                    {
                        unitItem = commonPosSubList[j];
                        int activeModeCount = activeModeIdes.Count(tcrId => unitItem.ActiveModeId == tcrId);

                        if (activeModeCount > 1)
                        {
                            IList<UnitItemData> commonActiveModeSubList = commonPosSubList.Where(ui => ui.ActiveModeId == unitItem.ActiveModeId).ToList();

                            IList<int> unitItemIdes = (from ui in commonPosSubList
                                                       select ui.Id).ToList();


                            for (int l = 0; l < commonActiveModeSubList.Count; l++)
                            {
                                unitItem = commonPosSubList[l];

                                int unitItemCount = unitItemIdes.Count(uiId => unitItem.Id == uiId);

                                if (unitItemCount > 1)
                                {
                                    IList<UnitItemData> commonUnitItemsSubList = commonActiveModeSubList.Where(ui => ui.Id == unitItem.Id).ToList();
                                    for (int k = 0; k < commonUnitItemsSubList.Count; k++)
                                    {
                                        UnitItemData deletedItem = commonUnitItemsSubList[k];
                                        commonActiveModeSubList.Remove(deletedItem);
                                        commonPosSubList.Remove(deletedItem);
                                        unitItemList.Remove(deletedItem);
                                    }
                                    unitItem.LastValue = commonUnitItemsSubList.Average(ui => ui.LastValue);
                                    commonActiveModeSubList.Add(unitItem);
                                }
                            }

                            // after making sure that all unit items now are aggregated // multiple units time
                            unitItem = commonActiveModeSubList[0];
                            activeModeCount = commonActiveModeSubList.Count;
                            if (activeModeCount > 1)
                            {
                                unitItem.SubLastValues = new double[activeModeCount];
                                unitItem.SubItemIdes = new int[activeModeCount];
                                for (int k = 0; k < activeModeCount; k++)
                                {
                                    UnitItemData deletedItem = commonActiveModeSubList[k];
                                    unitItem.SubLastValues[k] = deletedItem.LastValue;
                                    unitItem.SubItemIdes[k] = deletedItem.Id;

                                    commonPosSubList.Remove(deletedItem);
                                    unitItemList.Remove(deletedItem);

                                    // new if any is over flow
                                    if (deletedItem.LastValue >= deletedItem.AlertValue)
                                    {
                                        isOverFlow = true;
                                    }
                                }

                                unitItem.LastValue = commonActiveModeSubList.Average(ui => ui.LastValue);
                                UnitItemData unitItemDAL = (UnitItemData)unitItem;
                                unitItemDAL.IsOverflow = isOverFlow;
                                unitItemListFinal.Add(unitItemDAL);
                            }
                            else
                            {
                                unitItemListFinal.Add((UnitItemData)unitItem);
                            }

                        }
                        else
                        {
                            unitItemListFinal.Add((UnitItemData)unitItem);
                        }
                    }
                }
                else
                {
                    unitItemListFinal.Add((UnitItemData)unitItem);
                }
            }

            for (int i = 0; i < unitItemListFinal.Count; i++)
            {
                UnitItemData item = unitItemListFinal[i];

                if (!unitItemFilter.IsSubFilter && (item.LastValue < unitItemFilter.MinValue || item.LastValue > unitItemFilter.MaxValue))
                {
                    unitItemListFinal.RemoveAt(i);
                    i--;
                    continue;
                }

                if (item.LastValue >= item.AlertValue)
                {
                    item.IsOverflow = true;
                }
            }
            return unitItemListFinal;
        }


        [HttpGet("{id}")]
        public ActionResult<List<UnitItemDetails>> GetUnitItemDetails(int id)
        {
            List<UnitItemDetails> unitItemDetails = globalTypeRepository.GetUnitItemDetails(id);

            foreach (UnitItemDetails item in unitItemDetails)
            {
                if (item.LastValue >= item.AlertValue)
                {
                    item.IsOverflow = true;
                }
            }

            if (unitItemDetails == null)
            {
                return NotFound();
            }

            return unitItemDetails;
        }

    }
}
