import {useState, useEffect} from 'react'
import {useParams, Link} from 'react-router-dom'
import Error from '../Error'
import './css/details.css'
import Api from '../../services/blogPostApi'


function Details()
{
    const imageUrl = 'https://img.freepik.com/fotos-premium/modelo-de-jornal-dobrado-em-preto-e-branco-fundo-em-branco-para-modelo-de-pagina-de-noticias_1029476-75246.jpg'
    const { id } = useParams()
    const [post, setPost] = useState(null)

    useEffect(() => {

        async function getDetails(){
            try{
                const data = await Api(`Post/${id}`)
                setPost(data)
            }catch(error){
                console.error('Ocorreu um erro na requisição:', error)
            }
        }

        getDetails()

    },[id])

    if(!post)
        return(
        <>
            <Error />
        </>)

    return(
        <>
            <div className="post-detalhes-container" style={{ backgroundImage: `url(${imageUrl})` }}>
                <div className="post-detalhe-overlay">
                <h2 className="Apresentacao">O Seu Informativo Diário</h2>
            <div className="post-detalhe-conteudo">
                <div className="post-info-principal">
                    <h3 className="post-titulo">{post.title}</h3>
                    <p className="post-descricao">{post.content}</p>
                </div>
                <div className="post-info-lateral">
                    <div className="post-info-box">
                         <span className="post-info-label">Publicado em:</span>
                         <span className="post-info-valor">{post.publishedOn}</span>
                    </div>
                <div className="post-info-box">
                    <span className="post-info-label">Tipo de Conteúdo:</span>
                    <span className="post-info-valor">{post.typeContent}</span>
                </div>
                </div>
            </div>
             <Link to="/posts" className='post-voltar'>Voltar</Link>
             <br></br>
            </div>
        </div>
        </>
    )
}

export default Details