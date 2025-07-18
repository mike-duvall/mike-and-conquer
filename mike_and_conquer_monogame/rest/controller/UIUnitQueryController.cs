﻿using Microsoft.AspNetCore.Mvc;

using mike_and_conquer_monogame.gameview;
using mike_and_conquer_monogame.main;
using mike_and_conquer_monogame.rest.domain;


namespace mike_and_conquer_monogame.rest.controller
{
    [ApiController]
    //    [Route("[controller]")]

    [Route("ui/query/unit")]


    public class UIUnitQueryController : ControllerBase
    {


        [HttpGet]
        public ActionResult Get([FromQuery] int unitId)
        {

            UnitView unitView = MikeAndConquerGame.instance.GetUnitViewById(unitId);


            RestUnit restUnit = new RestUnit();

            restUnit.UnitId = unitView.UnitId;
            restUnit.Selected = unitView.Selected;
            restUnit.XInWorldCoordinates = unitView.XInWorldCoordinates;
            restUnit.YInWorldCoordinates = unitView.YInWorldCoordinates;

            return new OkObjectResult(restUnit);
        }



    }
}
