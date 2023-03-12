import React from "react";

function Home() {
    const BuildUrlParams = (obj) => {
        let str = ""
    
        for (const key in obj) {
            const value = obj[key];
            str += `${key}=${value}&`;
        }
    
        return str;
    };

    const generateSteamLoginUrl = (callbackUrl) => {
        const STEAM_LOGIN = "https://steamcommunity.com/openid/login";
        const params = {
            "openid.ns": "http://specs.openid.net/auth/2.0",
            "openid.mode": "checkid_setup",
            "openid.return_to": callbackUrl,
            "openid.realm": "http://localhost:3000",
            "openid.identity": "http://specs.openid.net/auth/2.0/identifier_select",
            "openid.claimed_id": "http://specs.openid.net/auth/2.0/identifier_select",
        };

        const url = `${STEAM_LOGIN}?${BuildUrlParams(params)}`
        return url;
    };
    
    const loginClick = () => {
        window.location.replace(generateSteamLoginUrl("http://localhost:3000/Authenticate"));
    };

    return (
        <div class="homeContainer">
            <button class="homeContainerItem" onClick={loginClick}>Login With Steam</button>
        </div>
    );
}

export default Home;