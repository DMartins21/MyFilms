import {useState, useEffect} from 'react'
import {useParams, Link} from 'react-router-dom'
import Error from '../Error'
import { toast } from 'react-toastify'
import Api from '../../services/filmesApi'

// const url = 'http://localhost:5290/api/Filme/Titulo'

function Details()
{
    const { id } = useParams()
    const [filme, setFilme] = useState(null)

    useEffect(() => {

        async function getDetails(){
            try{
                const data = await Api(id)
                setFilme(data)
            }catch(error){
                console.error('Ocorreu um erro na requisição:', error)
            }
        }

        getDetails()

    },[id])

    if(!filme)
        return(
        <>
            <Error />
        </>)
    function handleFav(){
        const listaFav = localStorage.getItem("favoritos")

        let favFilms = JSON.parse(listaFav) || []

        const hasFilm = favFilms.some((favFilms) => favFilms.id === filme.id)
        if(hasFilm){
            toast.warn("Filme já está na Lista de Favoritos")
            return
        }
        
        favFilms.push(filme)
        localStorage.setItem("favoritos", JSON.stringify(favFilms))
        toast.success("Filme Adicionado com Sucesso")
    }
    return(
        <>
            <div className="filme-detalhes-container" style={{ backgroundImage: `url(${filme.imageUrl})` }}>
                <div className="filme-detalhe-overlay">
                <h2 className="Apresentacao">Detalhes Do Filme | {filme.title}</h2>
            <div className="filme-detalhe-conteudo">
                <img src={filme.thumbnailUrl} alt={filme.title} className="filme-imagem"/>
                <div className="filme-info-principal">
                    <h3 className="filme-titulo">{filme.title}</h3>
                    <p className="filme-descricao">{filme.description}</p>
                </div>
                <div className="filme-info-lateral">
                    <div className="filme-info-box">
                         <span className="filme-info-label">Lançamento:</span>
                         <span className="filme-info-valor">{filme.releaseDate}</span>
                    </div>
                <div className="filme-info-box">
                    <span className="filme-info-label">Gênero:</span>
                    <span className="filme-info-valor">{Array.isArray(filme.genre) ? filme.genre.join(', ') : filme.g}</span>
                </div>
                </div>
            </div>
             <Link to="/filmes">Voltar</Link>
             <br></br>
             <button onClick={handleFav}>Favoritar</button>
            </div>
        </div>
        </>
    )
}

export default Details