import { Link } from 'react-router-dom'
import './css/styleHeader.css'
import NavBar from '../NavBar/navbar'




function Header()
{   
    return (
        <div>
            <header>
                <Link to="/"><h1>MyFilms.com</h1></Link>
                <p>O seu site de catalogo de filmes</p>
                <NavBar />
            </header>
        </div>

    )
}

export default Header