import {useState, useEffect} from 'react'
import {Link} from 'react-router-dom'
import Error from '../Error'
import Api from '../../services/blogPostApi'
import './css/index.css'

function Posts()
{

    const [posts, setPosts] = useState([])
    const [loading, setLoading] = useState(true)

    useEffect(() => {
        
        async function getPosts()
        {
            try{
                const data = await Api('BlogPosts')
                setPosts(data)
            }catch(error){
                console.error("Ocorreu um erro", error)
            }finally{
                setLoading(false)
            }
        }

        getPosts()

    },[])

        if(loading) {
            return(
                <>
                    <div className="posts-container">
                    <h2>Carregando...</h2>
                    </div>
                </>
            )
        }

        if(!posts || posts === undefined){
            return(
                <>
                   <Error/>
                </>
            )
        }

        return(
        <>
        <div className="posts-container">
            <h2>Posts Em Destaque</h2>

            <div className="posts-grid">
                {posts.map( post => (
                <article key={post.id} className='getPosts-Card'>
                    <h3 className='post-titulo'>{post.title}</h3>
                    <span className='post-type'>{post.typeContent}</span>
                    <Link to={`${post.id}`}>Ver Detalhes</Link>
                </article>

                ))}

            </div>
        </div>
        </>
    )
}

export default Posts