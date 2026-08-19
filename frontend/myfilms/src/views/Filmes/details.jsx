import {useState, useEffect} from 'react'
import {useParams, Link} from 'react-router-dom'

const url = 'http://localhost:5290/api/Filme/titulo'

function Details()
{
    const { title } = useParams()
    const [filme, setFilme] = useState(null)

    useEffect(() => {
        function getFilme()
        {
            fetch(`${url}/${title}`)
            .then(response => response.json())
            .then(json =>  setFilme(json))
            .catch(error => console.log(error))
        }

        getFilme()
    },[title])

    if(!filme) return <p>Carregando...</p>

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
                    <span className="filme-info-valor">{filme.genre.join(', ')}</span>
                </div>
                </div>
            </div>
             <Link to="/filmes">Voltar</Link>
            </div>
        </div>
        </>
    )
}

export default Details