import { useState, useEffect } from 'react';
import { Link, useNavigate } from 'react-router-dom';
import Api from '../../services/filmesApi';
import './style.css'
import { toast } from 'react-toastify';

function Search() {
    const [searchValue, setSearchValue] = useState([]);
    const [query, setQuery] = useState('');
    const navigate = useNavigate();


    useEffect(() => {

        if (query.trim() === '') {
            setSearchValue([]);
            return;
        }

        const timeOutId = setTimeout(async () => {
            try {
                const data = await Api('titulo', query);
                setSearchValue(data);
            } catch (error) {
                console.error('Ocorreu um erro na requisição:', error);
                setSearchValue([]);
                toast.error('Parece Não Temos Nada Aqui')
                navigate('/error');
            }
        }, 1000)

        return () => clearTimeout(timeOutId);

    }, [query])


    return (
        <>
            <div className="search-box">
                <div className="barra-pesquisa">
                    <input type="text" className="input-box" placeholder="pesquise aqui" value={query} onChange={(e) => setQuery(e.target.value)}></input>
                </div>

                {(searchValue ?? []).length > 0 && (
                    <div className="list-search">
                        {searchValue.map((filme) => (
                            <div key={filme.id} className="search-item">
                                <h3 className="search-title">{filme.title}</h3>
                                <img src={filme.thumbnailUrl} alt={filme.title} className="search-thumbnail" />
                                <Link to={`/filmes/${filme.id}`} className="search-link" onClick={(e) => setQuery('')} >Ver Detalhes</Link>
                            </div>
                        ))}
                    </div>
                )}
            </div>
        </>
    )

}

export default Search;