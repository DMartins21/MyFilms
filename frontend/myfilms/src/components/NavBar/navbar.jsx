import './css/style.css'
import { NavLink } from "react-router-dom"
import Search from "../Search"


function isTokenExpired(token){
        if(!token){
            return true
        }
        if(Date.now() > new Date(token).getTime()){
            return true
        }
        return false
    }

function NavBar(){

    const user = sessionStorage.getItem('user')
    const tokenEx = sessionStorage.getItem('expirationToken')

    return(
        <>
        <div className="Navs-pages">
             <NavLink to="/">Home</NavLink>
             <NavLink to="/filmes" >Todos os Filmes</NavLink>
                
             {!user || isTokenExpired(tokenEx)  ?
                (
                    <>
                        <NavLink to="/login">Login</NavLink>
                    </>
                ) : (
                     <>
                       <NavLink to='/user/favs'>Meus Favoritos</NavLink>
                       <NavLink to='/user/myUser'>Meu Usuário</NavLink>
                        <NavLink to='/logout'>Logout</NavLink>
                     </>       
                    )
            }                
        </div>

        <div className="searchFilm-area">
            <Search />
        </div>
        </>
    )
}

export default NavBar;