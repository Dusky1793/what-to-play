import React from "react";
import { useSearchParams, useNavigate } from "react-router-dom";
import { useState, useEffect } from "react";
import Games from '../Games';

function Authenticate() {
    const [searchParams, setSearchParams] = useSearchParams();
    const navigate = useNavigate();

    useEffect(() => {
        const steamOpenIdUrl = searchParams.get("openid.claimed_id").split("/");
        const steamId = steamOpenIdUrl[5];

        fetch(`${process.env.REACT_APP_API_URL}/Encryption/EncryptSteamId`, {
                method: 'POST',
                headers: {
                    'Accept': 'application/json',
                    'Content-Type': 'application/json'
                },
                body: JSON.stringify(steamId)
            })
            .then(res => res.json())
            .then((result) => {
                localStorage.setItem("steamId", result);
                navigate("/Games");
            });
    }, []);

    return (
        <>
        </>
    )
}

export default Authenticate;