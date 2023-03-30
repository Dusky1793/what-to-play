import React from "react";

function Home() {
    const BuildUrlParams = (obj:any) => {
        let str = ""
    
        for (const key in obj) {
            const value = obj[key];
            str += `${key}=${value}&`;
        }
    
        return str;
    };

    const generateSteamLoginUrl = (callbackUrl:string) => {
        const STEAM_LOGIN = `${process.env.REACT_APP_STEAM_COMMUNITY_URL}/openid/login`;
        const params = {
            "openid.ns": "http://specs.openid.net/auth/2.0",
            "openid.mode": "checkid_setup",
            "openid.return_to": callbackUrl,
            "openid.realm": process.env.REACT_APP_ROOT_URL,
            "openid.identity": "http://specs.openid.net/auth/2.0/identifier_select",
            "openid.claimed_id": "http://specs.openid.net/auth/2.0/identifier_select",
        };

        const url = `${STEAM_LOGIN}?${BuildUrlParams(params)}`
        return url;
    };
    
    const loginClick = () => {
        window.location.replace(generateSteamLoginUrl(`${process.env.REACT_APP_ROOT_URL}/Authenticate`));
    };

    return (
        <div className="homeContainer">
            <button className="homeContainerItem" onClick={loginClick}>Login With Steam</button>
        </div>
    );
}

export default Home;