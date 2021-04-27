using System.Collections.Generic;
using System.Linq;


    public class MapHandler
    {
        private readonly IReadOnlyCollection<string> agentGamePlayScene;

        private readonly int agentRolePlay;

        private int agentGameInPlay;

        private List<string> agentPlayScenes;

        public MapHandler(MapSet gameScene, int agentRoles)
        {
            agentGamePlayScene = gameScene.agentPlayableScenes;
            this.agentRolePlay = agentRoles;

            agentGameReinitialise();
        }


        public bool confirmGameFinalised => agentGameInPlay == agentRolePlay;

        public string agentSetsGame
        {
            get
            {
                if (confirmGameFinalised)
             {
                return null;
            }

                agentGameInPlay++;

                if (agentPlayScenes.Count == 0)

            {
                agentGameReinitialise();
            }

                string agentScene = agentPlayScenes[UnityEngine.Random.Range(0, agentPlayScenes.Count)];

                agentPlayScenes.Remove(agentScene);

                return agentScene;
            }
        }

        private void agentGameReinitialise() => agentPlayScenes = agentGamePlayScene.ToList();
    }
