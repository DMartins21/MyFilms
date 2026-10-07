import '../User/css/createUser.css'
import { useState } from "react";
import { toast } from "react-toastify";
import Api from "../../services/userApi";
import { useNavigate } from "react-router-dom";

function CreateUser(){
    const navigate = useNavigate()
    const [ name, setName ] = useState('');
    const [ lastName, setLastName ] = useState('');
    const [ profilePictureUrl, setProfilePictureUrl ] = useState('');
    const [ birthDate, setBirthDate ] = useState('');

    const createUser = async (name, lastName, profilePictureUrl, birthDate) => {
        try{
            await Api('newClient', {
                method : 'POST',
                headers: {
                    'Content-Type': 'application/json',
                    'Authorization': `Bearer ${sessionStorage.getItem('tokenSession')}`
                },
                body: JSON.stringify({
                    name,
                    lastName,
                    profilePictureUrl,
                    birthDate
                })
            });
            toast.success('Usuário Cadastrado!');
            navigate('/myUser');
        }catch(e){
            console.error(e)
            toast.error('Ops... Ocorreu um erro');
        }
    }


    const handleSubmit = async e => {
        e.preventDefault();
        await createUser(name, lastName, profilePictureUrl, birthDate);
    }

    return(
        <>
            <div className="createUser">
                <form className="formUser" onSubmit={handleSubmit}>
                    <h2>Informações Sobre seu Perfil</h2>
                    <label htmlFor='name'>Nome</label>
                    <input id='name' className="inputName" type="text" placeholder="Seu nome" value={name} onChange={(e) => setName(e.target.value)} required />
                    <label htmlFor='lastname'>Sobrenome</label>
                    <input id='lastname' className="inputLastName" type="text" placeholder="Seu Sobrenome" value={lastName} onChange={(e) => setLastName(e.target.value)} />
                    <label htmlFor='profilepic'>Foto de Perfil</label>
                    <input id='profilepic' className="inputProfilePicture" type="url" placeholder="Coloque uma URL de imagem" value={profilePictureUrl} onChange={(e) => setProfilePictureUrl(e.target.value)} />
                    <label htmlFor='birthdate'>Data de Nascimento</label>
                    <input id='birthdate' className="inputBirthDate" type="date" placeholder="Seu Aniversário" value={birthDate} onChange={(e) => setBirthDate(e.target.value)} />
                    <button type="submit" className="btn btn-submit">Criar Usuário</button>
                </form>
            </div>
        </>
    )
}

export default CreateUser