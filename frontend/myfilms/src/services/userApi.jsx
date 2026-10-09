async function UserApi(endpoint = '', options = {}) {
    
    const url = `http://localhost:5290/Client/${endpoint}`;

    try{

        const response = await fetch(url, options);

        if(response.status === 401){
            window.location.href = '/unauthorized';
            throw new Error(response.statusText);
        }
        if(response.status === 400 || response.status === 409 || response.status === 500){
            window.location.href = '/error';
            throw new Error(response.statusText);
        }

        if(response.status === 204){
            const text = response.text()
            
            if(!text) return null
            
            try{
                JSON.parse(text)
            }catch{
                return text
            }
        }
        const data = await response.json();

        return data;

    }catch(e){
        console.error('Ocorreu um erro durante a solicitação: ', e);
        throw e;
    }
}

export default UserApi;