import React from "react";
import { useState, useEffect } from "react";

function GameContainer(props) {
    const getImageUrl = () =>{
        const appId = props.gameDetails.appId;
        const imgIconUrl = props.gameDetails.img_Icon_Url;
        const imgUrl = `http://media.steampowered.com/steamcommunity/public/images/apps/${appId}/${imgIconUrl}.jpg`;
        return imgUrl;
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