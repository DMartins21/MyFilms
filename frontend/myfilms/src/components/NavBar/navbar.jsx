import './css/style.css'
import { NavLink } from "react-router-dom"
import Search from "../Search"

function NavBar(){

    const role = sessionStorage.getItem("role")

    return(
        <>
        <div className="Navs-pages">
            <NavLink to="/">Home</NavLink>
            <NavLink to="/filmes" >Todos os Filmes</NavLink>
            <NavLink to="/posts" >Posts</NavLink>
            
            {!role ? (
            <>
                <NavLink to="/login" >Login</NavLink>
                <NavLink to="/register" >Registrar</NavLink>    
            </>
             ) : (
            <>
                <NavLink to="/user/myUser" >Meu Perfil</NavLink>
                <NavLink to="/user/favs" >Meus Favoritos</NavLink>
                {role === 'Admin' || role === 'SuperAdmin' && (
                    <NavLink to="/filmes/submit" >Cadastrar Filme</NavLink>
                )}
                <NavLink to="/logout" >Logout</NavLink>
            </>
        
            )}

            {role === 'Admin' || role === 'SuperAdmin' && (
                <>
                <NavLink to="/filmes/submit" >Cadastrar Filme</NavLink>
                </>
            )}
    
        </div>

        <div className="searchFilm-area">
            <Search />
        </div>
        </>
    )
}

export default NavBar;