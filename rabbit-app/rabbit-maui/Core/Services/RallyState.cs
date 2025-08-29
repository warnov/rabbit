using rabbit_maui.Core.Models;

namespace rabbit_maui.Core.Services
{

    /// <summary>
    /// In-memory state for the active Rally and the selected Stage.
    /// Keeps the core operations to add stages/sections and select the current stage.
    /// </summary>
    public class RallyState
    {
        /// <summary>
        /// The active rally in memory.
        /// </summary>
        public Rally CurrentRally { get; set; } = new();

        /// <summary>
        /// The currently selected stage (nullable if none is selected).
        /// </summary>
        public Stage? CurrentStage { get; private set; }

        /// <summary>
        /// Resets the state with a new Rally instance.
        /// </summary>
        public void NewRally(string name = "New Rally")
        {
            CurrentRally = new Rally { Name = name };
            CurrentStage = null;
        }

        /// <summary>
        /// Adds a new stage (E{n}) to the current rally and selects it.
        /// </summary>
        public Stage AddStage()
        {
            var index = CurrentRally.Stages.Count + 1;
            var stage = new Stage { Id = $"E{index}", Name = $"Stage {index}" };
            CurrentRally.Stages.Add(stage);
            CurrentStage = stage;
            return stage;
        }

        /// <summary>
        /// Adds a new section (E{n}-T{m}) to the given stage.
        /// </summary>
        public Segment AddSegment(Stage stage, double distanceKm, double timeMin)
        {
            var stageNumber = CurrentRally.Stages.IndexOf(stage) + 1;
            var sectionIndex = stage.Segments.Count + 1;

            var section = new Segment
            {
                Id = $"E{stageNumber}-T{sectionIndex}",
                DistanceKm = distanceKm,
                TimeMin = timeMin
            };

            stage.Segments.Add(section);
            return section;
        }

        /// <summary>
        /// Selects a stage by its Id (e.g., "E1"). If not found, leaves CurrentStage unchanged.
        /// </summary>
        public void SelectStage(string stageId)
        {
            var found = CurrentRally.Stages.FirstOrDefault(s => s.Id == stageId);
            if (found is not null) CurrentStage = found;
        }

        public void SelectFirstStage()
        {
            CurrentStage = CurrentRally.Stages.FirstOrDefault();
        }



    }
}
