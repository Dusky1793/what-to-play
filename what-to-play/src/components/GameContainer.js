import React from "react";
import { useState, useEffect } from "react";

function GameContainer(props) {
    const getImageUrl = () =>{
        var appId = props.gameDetails.appId;
        var imgIconUrl = props.gameDetails.img_Icon_Url;
        return "http://media.steampowered.com/steamcommunity/public/images/apps/" + appId + "/" + imgIconUrl + ".jpg";
    };

    return (
        <div class="gameContainer">            
            <img src={getImageUrl()} />
            <div>
                {props.gameDetails.name}
            </div>
            <div>
                {props.gameDetails.playtime_Forever_Hours}
            </div>
            <div>
                {props.gameDetails.appId}
            </div>
        </div>
    );
}

export default GameContainer;