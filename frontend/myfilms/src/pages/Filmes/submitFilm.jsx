import { useState } from 'react';
import { useNavigate } from 'react-router-dom';
import { toast } from 'react-toastify';
import './css/submitFilm.css';
import  Api  from '../../services/filmesApi';

function SubmitFilm()
{
    // const [filme, setFilme] = useState([]);
    const [title, setTitle] = useState('');
    const [description, setDescription] = useState('');
    const [genre, setGenre] = useState(['']);
    const [releaseDate, setReleaseDate] = useState('');
    const [imageUrl, setImageUrl] = useState('');
    const [thumbnailUrl, setThumbnailUrl] = useState('');
    const navigate = useNavigate();

    const createFilm = async(filme) => {
        try{
            const data = await Api('CreateFilm', '', {
                method: 'POST',
                headers: {
                    'Content-Type': 'application/json',
                    'Authorization': `Bearer ${sessionStorage.getItem('tokenSession')}`
                },
                body: JSON.stringify({
                    title: filme.title,
                    description: filme.description,
                    genre: filme.genre,
                    releaseDate: filme.releaseDate,
                    imageUrl: filme.imageUrl,
                    thumbnailUrl: filme.thumbnailUrl
                })
            })
            console.log(data)
            toast.success("Filme Cadastrado com Sucesso")
            navigate('/filmes')
        }
        catch(e){
            console.error('Um erro ocorreu:', e)
            toast.error('Erro ao cadastrar filme')
        }
    }

    const handleSubmit = (e) => {
        e.preventDefault();
        createFilm({title, description, genre: Array.isArray(genre) ? genre.filter(Boolean) : [genre], releaseDate, imageUrl, thumbnailUrl});
    }

    return(
        <>
            <div className="film-page">
               <form className="form-submit" onSubmit={handleSubmit}>
                               <h2>Cadastrar Filme</h2>

                <label>Titulo</label>
                <input type= "text" placeholder="Titulo" value={title} onChange={(e) => setTitle(e.target.value)} required max-length="150"></input>
                
                <label>Descrição</label>
                <textarea id="description" rows="4" placeholder="Descrição" value={description} onChange={(e) => setDescription(e.target.value)} maxLength="500"></textarea>
                
                <label>Genero</label>
                <input type= "text" placeholder="Genero" value={genre} onChange={(e) => setGenre(e.target.value)} maxLength="255"></input>
                
                <label>Lançamento</label>
                <input type= "date" placeholder="Ano de Lançamento" value={releaseDate} onChange={(e) => setReleaseDate(e.target.value)} required></input>
                
                <label>Imagem URL</label>
                <input type= "text" placeholder="URL da Imagem" value={imageUrl} onChange={(e) => setImageUrl(e.target.value)}></input>
                
                <label>Thumbnail URL</label>
                <input type= "text" placeholder="URL do Thumbnail" value={thumbnailUrl} onChange={(e) => setThumbnailUrl(e.target.value)}></input>
                
                <button type="submit" className="btn btn-submit">Cadastrar</button>
               </form>
            </div>
        </>
    )

}

export default SubmitFilm;