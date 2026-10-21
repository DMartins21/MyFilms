import { useEffect } from 'react';
import { useNavigate } from 'react-router-dom';
import { toast } from 'react-toastify';
import Api from '../../services/loginApi';


function Logout() {

    const navigate = useNavigate();

    const logoutUser = async () => {
        try {
            await Api('Revoke', {
                method: 'POST',
                headers: {
                    'Authorization': `Bearer ${sessionStorage.getItem('tokenSession')}`
                },
            })
            }
            catch (e) {
            console.error('Um erro ocorreu:', e)
            toast.error(`Erro ao efetuar logout`)
            return null
        }
            sessionStorage.removeItem("user")
            sessionStorage.removeItem("tokenSession")
            sessionStorage.removeItem("refreshToken")
            sessionStorage.removeItem("expirationToken")
                        
            toast.success("Logout Efetuado com Sucesso", {toastId: 'logout'})
            setTimeout( () => {
                window.location.reload()
            }, 2500)
        
    }

    useEffect(() => {
        logoutUser()
        navigate('/')
    },[navigate])

}
export default Logout;