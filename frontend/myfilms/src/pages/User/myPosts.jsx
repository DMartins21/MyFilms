import './css/myPosts.css'
import Api from '../../services/userApi'
import PostsApi from '../../services/blogPostApi'
import { useState, useEffect } from 'react'
import { Link } from 'react-router-dom';
import { FaRegEdit } from 'react-icons/fa';
import { MdDelete  } from 'react-icons/md'
import { toast } from 'react-toastify';

function MyPosts()
{
    const [post, setPost] = useState([]);

    useEffect(() => {
    async function getMyPosts()
    {
        const data = await Api('MyPosts', {
            method: 'GET',
            headers: {
                'Content-Type': 'application/json',
                'Authorization': `Bearer ${sessionStorage.getItem('tokenSession')}`,
            }
        });
        setPost(data);
    }
     
    getMyPosts();
    },[setPost])

    async function handleDelete(id) {
        try{
            await PostsApi(`DeletePost/${id}`, {
            method: 'DELETE',
            headers: {
                'Authorization': `Bearer ${sessionStorage.getItem('tokenSession')}`
            },
        })

        toast.success('Post Deletado!')
        setTimeout(() => {
                window.location.reload()
            }, 5000)
    }catch(error){
            console.error(error)
            return
        }
    }

    if(post.length < 1){
        return(
            <>
                <div className='posts-list'>
                    <h2 className='no-posts'>Não Há Posts Aqui</h2>
                </div>
            </>
        )
    }

    return(
        <>
            <div className='posts-list'>
                <h2>Meus Posts</h2>
                {post.map( item => (
                <div className='post-cards' key={item.id}>
                    <ul className='list'>
                        <li className='item'>
                            <h4 className='title'>{item.title}</h4>
                            <span className='content'>{item.content}</span>
                            <p className='type-content'>{item.typeContent}</p>
                            <Link className='post-completo' to={`/posts/${item.id}`}>Ver Post Completo</Link>
                            <Link className='editar-post' to={`/posts/update/${item.id}`}><FaRegEdit/>Editar Conteúdo</Link>
                            <button onClick={() => handleDelete(item.id)}><MdDelete color='red' scale={14}></MdDelete>Delete</button>
                        </li>
                    </ul>
                </div>
                ))}
            </div>
        </>
    )
}

export default MyPosts;