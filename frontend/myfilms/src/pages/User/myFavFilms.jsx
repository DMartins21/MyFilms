import '../User/css/stylefav.css'
import Api from '../../services/userApi';
import { useEffect, useState } from 'react';
import { Link, useNavigate } from 'react-router-dom';
import { toast } from 'react-toastify';

function MyFavFilms(){

    const [favoritos, setFavoritos] = useState([]);
    const navigate = useNavigate()

    useEffect(() => {
        try{
            async function getFavFilms()
            {
                const data = await Api('MyFavorites', {
                    method: 'GET',
                    headers: {
                    'Content-Type': 'application/json',
                    'Authorization': `Bearer ${sessionStorage.getItem('tokenSession')}`
                    }
                });
                setFavoritos(data)
            }

            getFavFilms();
        }catch(e){
            console.error(e)
        }
    }, [setFavoritos])

    async function deleteFav(id){
        try{
            await Api(`removeFilm?idFilme=${id}`, {
                method: 'DELETE',
                headers: {
                    'Content-Type': 'application/json',
                    'Authorization': `Bearer ${sessionStorage.getItem('tokenSession')}`
                }
            })
            toast.info(`Film has been removed`)
            navigate('/user/myUser')
        }catch(e){
            console.error(e)
        }
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


export default MyFavFilms;