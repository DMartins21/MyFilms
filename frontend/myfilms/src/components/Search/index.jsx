import './style.css'

function Search(){
    return(
        <>
            <div className="search-box">
                <div className="barra-pesquisa">
                    <input type="text" className="input-box" placeholder="pesquise aqui"></input>
                </div>
                <div className="list-search">

                </div>
            </div>
        </>
    )
}

export default Search;