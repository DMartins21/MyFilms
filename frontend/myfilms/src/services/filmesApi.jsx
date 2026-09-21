async function FilmesApi(endpoint = '', value = ''){
    const apiUrl = value ? `http://localhost:5290/api/Filme/${endpoint}/${encodeURIComponent(value)}` : `http://localhost:5290/api/Filme/${endpoint}`

    try {

        const response = await fetch(apiUrl)

        if(!response.ok){
            throw new Error(response.statusText)
        }

        const data = await response.json()


        return data

    }catch (error) {
        console.error('Erro ao buscar dados da API:', error)
        throw error
    }
}

export default FilmesApi