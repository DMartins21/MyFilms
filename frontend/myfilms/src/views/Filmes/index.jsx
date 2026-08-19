import {useState, useEffect} from 'react'
import {Link} from 'react-router-dom'

const url = 'http://localhost:5290/api/Filme'

function Filmes()
{

    const [filmes, setFilmes] = useState([])

    useEffect(() => {
        function getFilme()
        {
            fetch(url)
            .then(response => response.json())
            .then(json =>  setFilmes(json))
            .catch(error => console.log(error))
        }

        getFilme()
    },[])
    
    return(
        <>
        <div className="filmes-container">
            <h2>Filmes Em Destaque</h2>
            <div className="filmes-grid">
                {filmes.map(filme =>{
                    return(
                        <article key={filme.id} className="getFilmes">
                            <h3 className="filme-titulo">{filme.title}</h3>
                            <p className="filme-descricao">{filme.description}</p>
                            <img src={filme.thumbnailUrl} alt={filme.title} />
                            <Link to={`/filmes/${filme.title}`}>Ver Detalhes</Link>
                        </article>
                    )
                    
                })}
            </div>

            
        </div>
        </>
    )
}

export default Filmes