
function App() {

  const user = localStorage.getItem("user")

  return (
    <>
    <div className="home" >
      <h1>Bem-vindo ao myfilms {user ? user : ""}</h1>
      <p>O myfilms é um projeto de catalogação de filmes.</p>
    </div>
    </>
  )
}

export default App
