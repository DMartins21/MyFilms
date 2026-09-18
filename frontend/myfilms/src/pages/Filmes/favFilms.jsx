import { useState,useEffect } from "react";
import { Link } from "react-router-dom";
import './stylefav.css'

function FavFilms(){
    const [favoritos, setFavoritos] = useState([])

    useEffect(() =>{
        try{
            function getFavoritos(){
                const favs = localStorage.getItem("favoritos")
                let filmes = JSON.parse(favs)
                setFavoritos(filmes || [])
            }

            getFavoritos()
        }catch(e){
            console.log(e)
        }
    }, [])

    function deleteFav(id){
        
        const myFav = JSON.parse(localStorage.getItem("favoritos"))
        
        let film = myFav.filter((item) => {
            return(
                item.id !== id
            )
            
        })
        console.log(film)

        setFavoritos(film)
        console.log(film)
        localStorage.setItem("favoritos", JSON.stringify(film))
    }

    if(favoritos.length == 0)
        return(
            <>
                <div className="favs">
                    <h2 className="titlePage">Meus Favoritos</h2>
                    <div className="noFavs">
                        <p>Sem Filmes</p>
                    </div>
                </div>
            </>
    )
    return(
        <>
        <div className="list">
            <h2 className="titlePage">Meus Favoritos</h2>
            {favoritos.map( item => (
                <div className="favs" key={item.id}>
                    <ul className="list-favs">
                        <li className="item">
                            <h4 className="favTitle">{item.title}</h4>
                            <p>{item.description}</p>
                            <img src={item.thumbnailUrl} alt={item.title}></img>
                            <Link to={`/filmes/${item.id}`}>Detalhes</Link>
                            <button onClick={() => deleteFav(item.id)}>Excluir da Lista</button>
                        </li>
                    </ul>
                </div>
            ))}
                
        </div>
        </>
    )
}

export default FavFilms