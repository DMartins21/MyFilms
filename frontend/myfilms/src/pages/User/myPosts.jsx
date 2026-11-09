import './css/myPosts.css'
import Api from '../../services/userApi'
import { useState, useEffect } from 'react'
import { Link } from 'react-router-dom';

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
    })


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
                <div className='post-cards'>
                    <ul className='list'>
                        <li className='item'>
                            <h4 className='title'>{item.title}</h4>
                            <span className='content'>{item.content}</span>
                            <p className='type-content'>{item.typeContent}</p>
                            <Link to={`/posts/${item.id}`}>Ver Post Completo</Link>
                        </li>
                    </ul>
                </div>
                ))}
            </div>
        </>
    )
}

export default MyPosts;