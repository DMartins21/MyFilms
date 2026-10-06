import { Link } from 'react-router-dom'
import './css/style.css'

function Unauthorized(){
    return(
        <>
            <div className="unauthorized-container">
                <div className="unauthorized-image">
                    <img src="https://cdn-icons-png.flaticon.com/512/1828/1828843.png" alt="Acesso Negado" />
                </div>

                <h2>Acesso Negado</h2>
                <p>Você não tem permissão para acessar esta página.</p>
                <Link to="/" className="btn-home">Voltar para a página inicial</Link>
            </div>
        </>
    )
}

export default Unauthorized