import { useEffect, useState } from "react";
import styles from "../styles/Home.module.css";
import Link from "next/link";
import { useRouter } from "next/router";

export default function Home() {
  const [message, setMessage] = useState("");
  const [auth, setAuth] = useState(false);
  const router = useRouter();

  useEffect(() => {
    (async () => {
      const response = await fetch("http://localhost:5127/api/Auth/user", {
        credentials: "include",
      });
      const content = await response.json();
      setMessage(content.name + ", ");
      setAuth(true);
      if (!content.name) {
        setMessage("Visitante, ");
        setAuth(false);
      }
    })();
  });

  const logout = async () => {
    await fetch("http://localhost:5127/api/Auth/logout", {
      method: "POST",
      headers: { "Content-Type": "application/json" },
      credentials: "include",
    });
    await router.push("/");
  };

  let authOptions;

  if (auth) {
    authOptions = (
      <>
        <div>
          <a href="#" onClick={logout}>
            Sair
          </a>
        </div>
      </>
    );
  }
  if (!auth) {
    authOptions = (
      <>
        <div className={styles.container}>
          <Link href="/login">
            <button className={styles.button}>Entre em sua conta!</button></Link>
          <br /><br />
          <Link href="/cadastro">
            <button className={styles.button}>
            Ainda não possui conta?
            </button>
            </Link>
        </div>
      </>
    );
  }

  return (
    <section className={styles.container}>
      <h1>
        {message} Bem-vindo(a) ao <span>Ponto Calculator</span>
      </h1>
      <p>Comece a gerenciar os seus turnos de trabalho agora mesmo!</p>
      {authOptions}
    </section>
  );
}
