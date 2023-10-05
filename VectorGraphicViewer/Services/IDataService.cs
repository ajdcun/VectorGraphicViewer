using System.IO;
using System.Threading.Tasks;
using VectorGraphicViewer.MVVM.ViewModel;

namespace VectorGraphicViewer.Services
{
    public interface IDataService
    {
        void SetFilePath(string fileName);
        Task<string> GetData();
    }

    public class JsonDataService : BaseViewModel, IDataService
    {
        private string _inputPath;
        private string _filePath;

        public string FilePath
        {
            get => _filePath;
            private set
            {
                _filePath = value;
                OnPropertyChanged(nameof(FilePath));
            }
        }

        public JsonDataService(string inputPath)
        {
            _inputPath = inputPath;
        }

        public void SetFilePath(string fileName)
        {
            FilePath = Path.Combine(_inputPath, fileName);
        }

        public async Task<string> GetData()
        {
            using (StreamReader reader = new StreamReader(FilePath))
            {
                return await reader.ReadToEndAsync();
            }
        }
    }
}
