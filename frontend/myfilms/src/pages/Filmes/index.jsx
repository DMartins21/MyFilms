import {useState, useEffect} from 'react'
import {Link} from 'react-router-dom'
import Error from '../Error'
import Api from '../../services/api'

function Filmes()
{

    const [filmes, setFilmes] = useState([])
    const [loading, setLoading] = useState(true)

    useEffect(() => {
        
        async function getFilme()
        {
            try{
                const data = await Api()
                setFilmes(data)
            }catch(error){
                console.error("Ocorreu um erro", error)
            }finally{
                setLoading(false)
            }
        }

        getFilme()

    },[])

        if(loading) {
            return(
                <>
                    <div className="filmes-container">
                    <h2>Carregando...</h2>
                    </div>
                </>
            )
        }

        if(!filmes || filmes === undefined){
            return(
                <>
                   <Error/>
                </>
            )
        }

        return(
        <>
        <div className="filmes-container">
            <h2>Filmes Em Destaque</h2>

            <div className="filmes-grid">
                {filmes.map( filme => (
                <article key={filme.id} className='getFilmes'>
                    <h3 className='filme-titulo'>{filme.title}</h3>
                    <p className='filme-descricao'>{filme.description}</p>
                    <img src={filme.thumbnailUrl} alt={filme.title}></img>
                    <Link to={`${filme.id}`}>Ver Detalhes</Link>
                </article>

                ))}

            </div>
        </div>
        </>
    )
}

export default Filmes