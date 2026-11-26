import { useParams } from 'react-router-dom'
import api from '../../services/blogPostApi'
import { useEffect, useState } from 'react';
import { toast } from 'react-toastify';
import './css/newpost.css'


function UpdatePost(){

    const { id } = useParams();
    const [post, setPost] = useState(null)
    const [title, setTitle] = useState('');
    const [content, setContent] = useState('');
    const [typeContent, setTypeContent] = useState('');

    async function update(){
        try {
             await api(`UpdatePost/${id}`, {
                method: 'PUT',
                headers: {
                    'Content-Type': 'application/json',
                    'Authorization': `Bearer ${sessionStorage.getItem('tokenSession')}`
                },
                body: JSON.stringify({ title, content, typeContent })
            });
        } catch (error) {
            console.error(error);
            return;
        }
        toast.info('Post Atualizado!');
    }


    useEffect(() => {
        async function getPost() {
            try {
                const data = await api(`Post/${id}`);
                setPost(data);
                setTitle(data.title)
                setContent(data.content)
                setTypeContent(data.typeContent)
            } catch (error) {
                console.error(error);
                return;
            }
        }

        getPost();
    
    }, [id])


    const handleSubmit = async(e) => {
        e.preventDefault();
        await update(title, content, typeContent);
    }

    return(
        <>
        <div className="newpost-card">
            <form className="newpost-infos" onSubmit={handleSubmit}>
                <span className="info-title">Informe o Titulo</span>
                <input className="input-value" type="text" placeholder="Titulo do Post" value={title} onChange={(e) => setTitle(e.target.value)} required></input>
                <textarea className="input-value-content" type="text" placeholder="Escreva seu post" value={content} onChange={(e) => setContent(e.target.value)} required></textarea>
                <input className="input-value-typeContent" type="text" placeholder="Tipo do conteúdo" value={typeContent} onChange={(e) => setTypeContent(e.target.value)} required></input>
                <button className="form-button" type="submit">Atualizar Post</button>
            </form>
        </div>
        </>
    )

}

export default  UpdatePost 