async function Api(endpoint = ''){
    const apiUrl = `http://localhost:5290/api/Filme/${endpoint}`

    try {

        const response = await fetch(apiUrl)

        if(!response.ok){
            throw new Error(response.statusText)
        }

        const data = await response.json()
        return data

    }catch (error) {
        console.error('Erro ao buscar dados da API:', error)
    }
}

export default Api