import React from "react";
import { useSearchParams, useNavigate } from "react-router-dom";
import { useState, useEffect } from "react";
import Games from '../Games';

function Authenticate() {
    const [searchParams, setSearchParams] = useSearchParams();
    const navigate = useNavigate();

    useEffect(() => {
        const steamOpenIdUrl = searchParams.get("openid.claimed_id").split("/");
        localStorage.setItem("steamId", steamOpenIdUrl[5]); // TODO: call the backend api and encrypt this steam id
        navigate("/Games");
    }, []);

    return (
        <>
        </>
    )
}

export default Authenticate;