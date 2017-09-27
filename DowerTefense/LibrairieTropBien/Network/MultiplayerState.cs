namespace LibrairieTropBien.Network
{
    /// <summary>
    /// Énumérateur des différents états de connexion / recheche de match / en match...
    /// </summary>
    public enum MultiplayerState
    {
        Disconnected, // Mode hors ligne
        Connected, // Connexion au service d'authentification
        Authentified, // Connecté à un compte
        SearchingGame, // En recherche de match
        InLobby, // Dans un lobby
        InGame, // En jeu
        InEndGameLobby, // En récap de fin de match

    }
}
