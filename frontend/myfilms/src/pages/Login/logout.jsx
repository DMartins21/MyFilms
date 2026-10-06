import { useEffect } from 'react';
import { useNavigate } from 'react-router-dom';
import { toast } from 'react-toastify';


function Logout() {

    const navigate = useNavigate();

    const logoutUser = async () => {
        try {
            sessionStorage.removeItem("user")
            sessionStorage.removeItem("tokenSession")
            sessionStorage.removeItem("refreshToken")
            sessionStorage.removeItem("expirationToken")
            navigate('/')
            toast.success("Logout Efetuado com Sucesso")
            }
            catch (e) {
            console.error('Um erro ocorreu:', e)
            toast.error(`Erro ao efetuar logout`)
            return null
        }
    }

    useEffect(() => {
        logoutUser()
    }, [])

}
export default Logout;