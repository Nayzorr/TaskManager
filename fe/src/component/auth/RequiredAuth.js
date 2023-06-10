import { useLocation, Navigate, Outlet } from "react-router-dom";
import UseAuth from "../../hooks/UseAuth";
import { useCookies } from 'react-cookie';
import API from "../../api/axios";

export default function RequiredAuth() {
    const {auth} = UseAuth();
    const location = useLocation();
    const [cookies, setCookie] = useCookies(['accessToken']);

    API.defaults.headers.common['Authorization'] = `Bearer ${cookies?.accessToken}`;

    console.log(auth);
    return (
        cookies?.accessToken
           ?<Outlet/>
           : cookies?.accessToken
                ? <Navigate to ="/unauthorized" state ={{from: location}} replace/>
                : <Navigate to="/login" state ={{from: location}} replace/>
    );
}