import { use, useEffect, useState } from 'react';
import { Link, useNavigate } from 'react-router-dom';
import Api from '../../services/loginApi';
import Error from '../Error';
import './style.css';


function Login() {
    const [userName, setUserName] = useState('');
    const [password, setPassword] = useState('');
    const [errorMessage, setErrorMsg] = useState('')
    const navigate = useNavigate()


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
            
            console.log('Login success', data)
            localStorage.setItem("user", userName)
            navigate('/')
            return data

        } catch (e) {
            console.error('Um erro ocorreu:', e)
            setErrorMsg('Usuário ou senha Inválidos')
            return null
        }
    }

    useEffect(() => {
        if(errorMessage){
            const timer = setTimeout(() => setErrorMsg(''), 1500)
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

        {errorMessage && (
            <div className="error-banner">
                {errorMessage}
                <button className="error-banner-close" onClick={() => setErrorMsg('')}>
                </button>
            </div>
        )}

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