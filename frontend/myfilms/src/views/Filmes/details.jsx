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
            <div className="filme-detalhes-container">
                <h2 className="Apresentacao">Detalhes Do Filme {filme.title}</h2>
                <img src={filme.imageUrl} alt={filme.title} className="filme-imagem"/>
                <h3 className="filme-titulo">{filme.title}</h3>
                <p className="filme-descricao">{filme.description}</p>
                <p className="filme-ano">{filme.releaseYear}</p>
                <p className="filme-genero">{filme.genre}</p>
                <Link to="/filmes">Voltar</Link>
            </div>
        </>
    )
}

export default Details