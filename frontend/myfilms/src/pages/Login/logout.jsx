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
            sessionStorage.removeItem("tokenSession")
            sessionStorage.removeItem("role")

            setTimeout(() => {
                window.location.reload()
            }, 3500)
            toast.success("Logout Efetuado com Sucesso", {toastId: 'logout'})

        
    }

    useEffect(() => {
        logoutUser()
        navigate('/', { replace: true })
    },[navigate])

}
export default Logout;