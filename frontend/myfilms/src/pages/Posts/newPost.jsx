import './css/newPost.css'
import { useState } from "react";
import blogPost from '../../services/blogPostApi'
import { useNavigate } from "react-router-dom";
import { toast } from "react-toastify";

function NewPost(){
    const [title, setTitle] = useState('');
    const [content, setContent] = useState('');
    const [typeContent, setTypeContent] = useState('');

    const navigate = useNavigate();

    async function createPost(title, content, typeContent){
        try{
       await blogPost('NewPost', {
            method: 'POST',
            headers: {
            'Content-Type': 'application/json',
            'Authorization': `Bearer ${sessionStorage.getItem('tokenSession')}`
            },
            body: JSON.stringify({
                title,
                content,
                typeContent
                })
            })
        }catch(e){
            console.error(e);
            return;
        }
        toast.success('post criado com sucesso')
        navigate('/posts')
    }

    const handleSubmit = async(e) =>{
        e.preventDefault()
        await createPost(title, content, typeContent);
    }



    return(
        <>
        <div className="newpost-card">
            <form className="newpost-infos" onSubmit={handleSubmit}>
                <span className="info-title">Informe o Titulo</span>
                <input className="input-value" type="text" placeholder="Titulo do Post" value={title} onChange={(e) => setTitle(e.target.value)} required></input>
                <textarea className="input-value-content" type="text" placeholder="Escreva seu post" value={content} onChange={(e) => setContent(e.target.value)} required></textarea>
                <input className="input-value-typeContent" type="text" placeholder="Tipo do conteúdo" value={typeContent} onChange={(e) => setTypeContent(e.target.value)} required></input>
                <button className="form-button" type="submit">Enviar Post</button>
            </form>
        </div>
        </>
    )
}

export default NewPost;