﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.18444
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace CheckersGame.CheckersService {
    using System.Runtime.Serialization;
    using System;
    
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Runtime.Serialization", "4.0.0.0")]
    [System.Runtime.Serialization.DataContractAttribute(Name="SimpleMove", Namespace="http://schemas.datacontract.org/2004/07/CheckersService")]
    [System.SerializableAttribute()]
    public partial class SimpleMove : object, System.Runtime.Serialization.IExtensibleDataObject, System.ComponentModel.INotifyPropertyChanged {
        
        [System.NonSerializedAttribute()]
        private System.Runtime.Serialization.ExtensionDataObject extensionDataField;
        
        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private string IDField;
        
        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private System.Collections.Generic.Dictionary<int, System.Collections.Generic.KeyValuePair<int, int>> JumpedSquaresField;
        
        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private bool Player1Field;
        
        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private System.Collections.Generic.Dictionary<int, System.Collections.Generic.KeyValuePair<int, int>> SquarePathField;
        
        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private System.Collections.Generic.KeyValuePair<int, int> StartSquareField;
        
        [global::System.ComponentModel.BrowsableAttribute(false)]
        public System.Runtime.Serialization.ExtensionDataObject ExtensionData {
            get {
                return this.extensionDataField;
            }
            set {
                this.extensionDataField = value;
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public string ID {
            get {
                return this.IDField;
            }
            set {
                if ((object.ReferenceEquals(this.IDField, value) != true)) {
                    this.IDField = value;
                    this.RaisePropertyChanged("ID");
                }
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public System.Collections.Generic.Dictionary<int, System.Collections.Generic.KeyValuePair<int, int>> JumpedSquares {
            get {
                return this.JumpedSquaresField;
            }
            set {
                if ((object.ReferenceEquals(this.JumpedSquaresField, value) != true)) {
                    this.JumpedSquaresField = value;
                    this.RaisePropertyChanged("JumpedSquares");
                }
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public bool Player1 {
            get {
                return this.Player1Field;
            }
            set {
                if ((this.Player1Field.Equals(value) != true)) {
                    this.Player1Field = value;
                    this.RaisePropertyChanged("Player1");
                }
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public System.Collections.Generic.Dictionary<int, System.Collections.Generic.KeyValuePair<int, int>> SquarePath {
            get {
                return this.SquarePathField;
            }
            set {
                if ((object.ReferenceEquals(this.SquarePathField, value) != true)) {
                    this.SquarePathField = value;
                    this.RaisePropertyChanged("SquarePath");
                }
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public System.Collections.Generic.KeyValuePair<int, int> StartSquare {
            get {
                return this.StartSquareField;
            }
            set {
                if ((this.StartSquareField.Equals(value) != true)) {
                    this.StartSquareField = value;
                    this.RaisePropertyChanged("StartSquare");
                }
            }
        }
        
        public event System.ComponentModel.PropertyChangedEventHandler PropertyChanged;
        
        protected void RaisePropertyChanged(string propertyName) {
            System.ComponentModel.PropertyChangedEventHandler propertyChanged = this.PropertyChanged;
            if ((propertyChanged != null)) {
                propertyChanged(this, new System.ComponentModel.PropertyChangedEventArgs(propertyName));
            }
        }
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Runtime.Serialization", "4.0.0.0")]
    [System.Runtime.Serialization.DataContractAttribute(Name="GameState", Namespace="http://schemas.datacontract.org/2004/07/CheckersService")]
    [System.SerializableAttribute()]
    public partial class GameState : object, System.Runtime.Serialization.IExtensibleDataObject, System.ComponentModel.INotifyPropertyChanged {
        
        [System.NonSerializedAttribute()]
        private System.Runtime.Serialization.ExtensionDataObject extensionDataField;
        
        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private CheckersGame.CheckersService.GameState.State GameInfoField;
        
        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private System.DateTime GameStartTimeField;
        
        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private string HostTeamNameField;
        
        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private CheckersGame.CheckersService.SimpleMove[] MoveListField;
        
        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private string OtherTeamNameField;
        
        [global::System.ComponentModel.BrowsableAttribute(false)]
        public System.Runtime.Serialization.ExtensionDataObject ExtensionData {
            get {
                return this.extensionDataField;
            }
            set {
                this.extensionDataField = value;
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public CheckersGame.CheckersService.GameState.State GameInfo {
            get {
                return this.GameInfoField;
            }
            set {
                if ((this.GameInfoField.Equals(value) != true)) {
                    this.GameInfoField = value;
                    this.RaisePropertyChanged("GameInfo");
                }
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public System.DateTime GameStartTime {
            get {
                return this.GameStartTimeField;
            }
            set {
                if ((this.GameStartTimeField.Equals(value) != true)) {
                    this.GameStartTimeField = value;
                    this.RaisePropertyChanged("GameStartTime");
                }
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public string HostTeamName {
            get {
                return this.HostTeamNameField;
            }
            set {
                if ((object.ReferenceEquals(this.HostTeamNameField, value) != true)) {
                    this.HostTeamNameField = value;
                    this.RaisePropertyChanged("HostTeamName");
                }
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public CheckersGame.CheckersService.SimpleMove[] MoveList {
            get {
                return this.MoveListField;
            }
            set {
                if ((object.ReferenceEquals(this.MoveListField, value) != true)) {
                    this.MoveListField = value;
                    this.RaisePropertyChanged("MoveList");
                }
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public string OtherTeamName {
            get {
                return this.OtherTeamNameField;
            }
            set {
                if ((object.ReferenceEquals(this.OtherTeamNameField, value) != true)) {
                    this.OtherTeamNameField = value;
                    this.RaisePropertyChanged("OtherTeamName");
                }
            }
        }
        
        public event System.ComponentModel.PropertyChangedEventHandler PropertyChanged;
        
        protected void RaisePropertyChanged(string propertyName) {
            System.ComponentModel.PropertyChangedEventHandler propertyChanged = this.PropertyChanged;
            if ((propertyChanged != null)) {
                propertyChanged(this, new System.ComponentModel.PropertyChangedEventArgs(propertyName));
            }
        }
        
        [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Runtime.Serialization", "4.0.0.0")]
        [System.Runtime.Serialization.DataContractAttribute(Name="GameState.State", Namespace="http://schemas.datacontract.org/2004/07/CheckersService")]
        public enum State : int {
            
            [System.Runtime.Serialization.EnumMemberAttribute()]
            WaitingForPlayers = 0,
            
            [System.Runtime.Serialization.EnumMemberAttribute()]
            InProgress = 1,
            
            [System.Runtime.Serialization.EnumMemberAttribute()]
            Ended = 2,
            
            [System.Runtime.Serialization.EnumMemberAttribute()]
            HostDisconnected = 3,
            
            [System.Runtime.Serialization.EnumMemberAttribute()]
            PlayerDisconnected = 4,
        }
    }
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    [System.ServiceModel.ServiceContractAttribute(ConfigurationName="CheckersService.ICheckersService")]
    public interface ICheckersService {
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/ICheckersService/AddMove", ReplyAction="http://tempuri.org/ICheckersService/AddMoveResponse")]
        bool AddMove(CheckersGame.CheckersService.SimpleMove move, int gameId);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/ICheckersService/GetLatestMoveFrom", ReplyAction="http://tempuri.org/ICheckersService/GetLatestMoveFromResponse")]
        CheckersGame.CheckersService.SimpleMove GetLatestMoveFrom(int gameId);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/ICheckersService/CreateGame", ReplyAction="http://tempuri.org/ICheckersService/CreateGameResponse")]
        int CreateGame(string hostTeamName);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/ICheckersService/JoinGame", ReplyAction="http://tempuri.org/ICheckersService/JoinGameResponse")]
        void JoinGame(int gameId, string otherTeamName);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/ICheckersService/Disconnect", ReplyAction="http://tempuri.org/ICheckersService/DisconnectResponse")]
        void Disconnect(int gameId, bool host);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/ICheckersService/EndGame", ReplyAction="http://tempuri.org/ICheckersService/EndGameResponse")]
        void EndGame(int gameId);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/ICheckersService/GetAllGames", ReplyAction="http://tempuri.org/ICheckersService/GetAllGamesResponse")]
        CheckersGame.CheckersService.GameState[] GetAllGames();
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/ICheckersService/GetSpecificGames", ReplyAction="http://tempuri.org/ICheckersService/GetSpecificGamesResponse")]
        System.Collections.Generic.Dictionary<int, CheckersGame.CheckersService.GameState> GetSpecificGames(CheckersGame.CheckersService.GameState.State state);
    }
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    public interface ICheckersServiceChannel : CheckersGame.CheckersService.ICheckersService, System.ServiceModel.IClientChannel {
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    public partial class CheckersServiceClient : System.ServiceModel.ClientBase<CheckersGame.CheckersService.ICheckersService>, CheckersGame.CheckersService.ICheckersService {
        
        public CheckersServiceClient() {
        }
        
        public CheckersServiceClient(string endpointConfigurationName) : 
                base(endpointConfigurationName) {
        }
        
        public CheckersServiceClient(string endpointConfigurationName, string remoteAddress) : 
                base(endpointConfigurationName, remoteAddress) {
        }
        
        public CheckersServiceClient(string endpointConfigurationName, System.ServiceModel.EndpointAddress remoteAddress) : 
                base(endpointConfigurationName, remoteAddress) {
        }
        
        public CheckersServiceClient(System.ServiceModel.Channels.Binding binding, System.ServiceModel.EndpointAddress remoteAddress) : 
                base(binding, remoteAddress) {
        }
        
        public bool AddMove(CheckersGame.CheckersService.SimpleMove move, int gameId) {
            return base.Channel.AddMove(move, gameId);
        }
        
        public CheckersGame.CheckersService.SimpleMove GetLatestMoveFrom(int gameId) {
            return base.Channel.GetLatestMoveFrom(gameId);
        }
        
        public int CreateGame(string hostTeamName) {
            return base.Channel.CreateGame(hostTeamName);
        }
        
        public void JoinGame(int gameId, string otherTeamName) {
            base.Channel.JoinGame(gameId, otherTeamName);
        }
        
        public void Disconnect(int gameId, bool host) {
            base.Channel.Disconnect(gameId, host);
        }
        
        public void EndGame(int gameId) {
            base.Channel.EndGame(gameId);
        }
        
        public CheckersGame.CheckersService.GameState[] GetAllGames() {
            return base.Channel.GetAllGames();
        }
        
        public System.Collections.Generic.Dictionary<int, CheckersGame.CheckersService.GameState> GetSpecificGames(CheckersGame.CheckersService.GameState.State state) {
            return base.Channel.GetSpecificGames(state);
        }
    }
}