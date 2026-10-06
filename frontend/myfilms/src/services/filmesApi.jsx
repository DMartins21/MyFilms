async function FilmesApi(endpoint = '', value = '', options = {}){
    const apiUrl = value ? `http://localhost:5290/api/Filme/${endpoint}/${encodeURIComponent(value)}` : `http://localhost:5290/api/Filme/${endpoint}`

    try {

        const response = await fetch(apiUrl, options)

        if(response.status === 403){
            window.location.href = '/unauthorized'
            return
        }

        if(!response.ok){
            throw new Error(data.message || response.statusText)
        }

        const data = await response.json()
        
        return data

    }catch (error) {
        console.error('Erro ao buscar dados da API:', error)
        throw error
    }
}

export default FilmesApi