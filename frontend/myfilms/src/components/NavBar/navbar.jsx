import './css/style.css'
import { NavLink } from "react-router-dom"
import Search from "../Search"
import { CgProfile } from 'react-icons/cg'
import { CiLogout } from 'react-icons/ci'

function NavBar(){

    const role = sessionStorage.getItem("role")
    const allowedRules = ['admin', 'superadmin']
    const isAuthorized = role && allowedRules.includes(role.toLocaleLowerCase())

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
                {isAuthorized && (
                    <NavLink to="/filmes/submit" >Cadastrar Filme</NavLink>
                )}
                <NavLink to="/user/myUser" ><CgProfile size={14} color='c8102e' display={'inline-flex'}></CgProfile>Meu Perfil</NavLink>
                <NavLink to="/logout" ><CiLogout></CiLogout>Logout</NavLink>
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