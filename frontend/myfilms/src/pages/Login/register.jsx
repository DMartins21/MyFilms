import { use, useEffect, useState } from 'react';
import { data, useNavigate } from 'react-router-dom';
import Api from '../../services/loginApi';
import { toast } from 'react-toastify';
import './css/loginStyle.css';


function Register() {
    const [userName, setUserName] = useState('');
    const [email, setEmail] = useState('')
    const [password, setPassword] = useState('');
    const [errorMessage, setErrorMsg] = useState('')
    const navigate = useNavigate()


    const registerUser = async (userName, email, password) => {
        try {
            const data = await Api('Register', {
                method: 'POST',
                headers: {
                    'Content-Type': 'application/json',
                },
                body: JSON.stringify({
                    userName,
                    email,
                    password
                }),
            })
            
            console.log('Register success', data)
            navigate('/Login')
            toast.success("Usuário Registrado")

        } catch (e) {
            console.error('Um erro ocorreu:', e)
            setErrorMsg(`${e}`)
            toast.error(e.message)
        }
    }

    useEffect(() => {
        if(errorMessage){
            const timer = setTimeout(() => setErrorMsg(''), 1500)
            return () => clearTimeout(timer)
        }
    }, [errorMessage])

    function validaSenha(senha){
        const regras = {
            tamanhoMinimo: senha.length > 6,
            temMinuscula: /[a-z]/.test(senha),
            temMaiuscula: /[A-Z]/.test(senha),
            temDigito:  /\d/.test(senha),
            temCaractereEspecial: /[^a-zA-Z0-9]/.test(senha)
        }

        const valida = Object.values(regras).every(Boolean)
        return {valida, regras}
    }

    const handleSubmit = async e => {
        e.preventDefault()
        
        const senhaIsValid = validaSenha(password)

        if(!senhaIsValid.valida){
            setErrorMsg('Erro ao Validar Senha', senhaIsValid.regras)
            toast.error('Senha Inválida')
            return
        }

        const registerData = await registerUser(userName, email , password)
        if (!registerData) {
            return
        }
    }

    return (
        <>

            <div className="login-page">
                <form className="login-container" onSubmit={handleSubmit}>
                    <h2 className="form-title">Registre-se no MyFilms</h2>
                    <h4 className="form-label">Username</h4>
                    <input type="text" placeholder="Informe seu username" value={userName} onChange={(e) => setUserName(e.target.value)} required></input>
                    <h4 className="form-label">Email</h4>
                    <input type="email" placeholder="Informe seu email" value={email} onChange={(e) => setEmail(e.target.value)} required></input>
                    <h4 className="form-label">Password</h4>
                    <input type="password" placeholder="Informe sua senha" value={password} onChange={(e) => setPassword(e.target.value)} required></input>
                    <br></br>
                    <button className="form-button" type="submit">
                        Registrar
                    </button>
                </form>
            </div>
        </>
    )
}

export default Register;