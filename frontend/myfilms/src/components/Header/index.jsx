import { Link } from 'react-router-dom'

function Header()
{
    return (
        <div>
            <header>
                <h1>MyFilms.com</h1>
                <p>O seu site de catalogo de filmes</p>
                <Link to="/">Home</Link>
                <Link to="/sobre">Sobre</Link  >
                <Link to="/contato">Contato</Link>
                <Link to="/filmes" >Todos os Filmes</Link>
            </header>
        </div>
    )
}

export default Header