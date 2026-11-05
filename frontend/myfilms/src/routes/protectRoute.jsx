import {jwtDecode} from 'jwt-decode'
import { Outlet, Navigate } from 'react-router-dom';


function isTokenExpired(tokenExpiration){
    if(tokenExpiration * 1000 < Date.now())
        return true
    return false
}

function userInfo(decodedToken)
{
    const role = decodedToken.role

    sessionStorage.setItem("role", role)
}

function ProtectRoute({ allowedRoles }) {
    const token = sessionStorage.getItem('tokenSession');

    if(!token)
        return <Navigate to = '/login' replace />

    let decoded;
    try{
        decoded = jwtDecode(token)
        userInfo(decoded)
    }catch(e){
        console.error('Erro ao decodificar o token:', e);
        return <Navigate to = '/login' replace />
    }

    if(isTokenExpired(decoded.exp))
        return <Navigate to = '/login' replace />

    if(allowedRoles)
    {
        const roles = Array.isArray(decoded.role) ? decoded.role : [decoded.role];
        if(!roles.some(role => allowedRoles.includes(role)))
            return <Navigate to = '/unauthorized' replace />
    }
    return <Outlet />;

}

export default ProtectRoute;