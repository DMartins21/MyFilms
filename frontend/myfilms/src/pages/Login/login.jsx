import { useEffect, useState } from 'react';
import { Link, useNavigate, Navigate } from 'react-router-dom';
import Api from '../../services/loginApi';
import { toast } from 'react-toastify';
import './css/loginStyle.css';


function Login() {
    const [userName, setUserName] = useState('');
    const [password, setPassword] = useState('');
    const [errorMessage, setErrorMsg] = useState('');
    const navigate = useNavigate();


    const loginUser = async (userName, password) => {
        try {
            const data = await Api('Login', {
                method: 'POST',
                headers: {
                    'Content-Type': 'application/json',
                },
                body: JSON.stringify({
                    userName,
                    password
                }),
            })
            
            // console.log('Login success', data)
            sessionStorage.setItem("user", userName.replace("_", " "))
            sessionStorage.setItem("tokenSession", data.token)
            sessionStorage.setItem("refreshToken", data.refreshToken)
            sessionStorage.setItem("expirationToken", data.expiration)
            navigate('/')
            toast.success("Login Efetuado com Sucesso")
            setTimeout(() => {
                window.location.reload()
            }, 3000);

        } catch (e) {
            console.error('Um erro ocorreu:', e)
            setErrorMsg('Usuário ou senha Inválidos')
            toast.error(`Usuário ou senha Inválidos`)
            return null
        }
    }

    useEffect(() => {

        if(errorMessage){
            const timer = setTimeout(() => setErrorMsg(''), 3000)
            return () => clearTimeout(timer)
        }
    }, [errorMessage])

    const handleSubmit = async e => {
        e.preventDefault()
        const loginData = await loginUser(userName, password)
        if (!loginData) {
            console.error(errorMessage)
        }
    }

    return (
        <>
            <div className="login-page">
                <form className="login-container" onSubmit={handleSubmit}>
                    <h2 className="form-title">Bem Vindo ao MyFilms</h2>
                    <h4 className="form-label">Username</h4>
                    <input type="text" placeholder="Informe seu username" value={userName} onChange={(e) => setUserName(e.target.value)} required></input>
                    <h4 className="form-label">Password</h4>
                    <input type="password" placeholder="Informe sua senha" value={password} onChange={(e) => setPassword(e.target.value)} required></input>
                    <br></br>
                    <Link to={'/register'} className="link">Criar Conta</Link>
                    <button className="form-button" type="submit">
                        Login
                    </button>
                </form>
            </div>
        </>
    )
}

export default Login;