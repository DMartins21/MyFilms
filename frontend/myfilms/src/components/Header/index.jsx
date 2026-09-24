import { Link } from 'react-router-dom'
import Search from '../Search/index'

function Header()
{
    const user = sessionStorage.getItem('user')
    const tokenEx = sessionStorage.getItem('expirationToken')

    return (
        <div>
            <header>
                <Link to="/"><h1>MyFilms.com</h1></Link>
                <p>O seu site de catalogo de filmes</p>
                <Link to="/">Home</Link>
                <Link to="/filmes" >Todos os Filmes</Link>
                
                {!user || Date.UTC.now < tokenEx  ?
                    (
                        <>
                        <Link to="/login" >Login</Link>
                        </>
                    ) : (
                     <>
                       <Link to='/filmes/favFilms'>Meus Favoritos</Link>
                     </>       
                    )
                }                
             

                <Search />
            </header>
        </div>
    )
}

export default Header