using System;
using System.Linq;
using Scrap.Core.Classes.Documents;
using Scrap.Core.Classes.References;

namespace Scrap.Models.Documents
{
    public sealed class ProcessingItemWrapper : BaseDocumentItemWrapper<ProcessingItem>
    {
        private Nomenclature _inputNomenclature;
        private double _inputWeight;
        private Nomenclature _outputNomenclature;
        private double _outputWeight;

        /// <summary>
        /// Конструктор
        /// </summary>
        /// <param name="processingItem"></param>
        public ProcessingItemWrapper(ProcessingItem processingItem = null)
            : base(processingItem)
        {
            if (processingItem != null)
            {
                Id = Container.Id;
                InputNomenclature =
                    MainStorage.Instance.Nomenclatures.FirstOrDefault(x => x.Id == Container.InputNomenclatureId);
                InputWeight = Container.InputWeight;
                OutputNomenclature =
                    MainStorage.Instance.Nomenclatures.FirstOrDefault(x => x.Id == Container.OutputNomenclatureId);
                OutputWeight = Container.OutputWeight;
            }
            else
            {
                Id = Guid.NewGuid();
                InputNomenclature = MainStorage.Instance.Nomenclatures.OrderBy(x => x.Name).FirstOrDefault();
                OutputNomenclature = MainStorage.Instance.Nomenclatures.OrderBy(x => x.Name).FirstOrDefault();
            }
        }

        /// <summary>
        /// Номенклатура на входе
        /// </summary>
        public Nomenclature InputNomenclature
        {
            get { return _inputNomenclature; }
            set
            {
                if (Equals(value, _inputNomenclature))
                    return;
                _inputNomenclature = value;
                RaisePropertyChanged("InputNomenclature");
            }
        }

        /// <summary>
        /// Масса на входе
        /// </summary>
        public double InputWeight
        {
            get { return _inputWeight; }
            set
            {
                if (value.Equals(_inputWeight))
                    return;
                _inputWeight = value;
                RaisePropertyChanged("InputWeight");
            }
        }

        /// <summary>
        /// Номенклатура на выходе
        /// </summary>
        public Nomenclature OutputNomenclature
        {
            get { return _outputNomenclature; }
            set
            {
                if (Equals(value, _outputNomenclature))
                    return;
                _outputNomenclature = value;
                RaisePropertyChanged("OutputNomenclature");
            }
        }

        /// <summary>
        /// Масса на выходе
        /// </summary>
        public double OutputWeight
        {
            get { return _outputWeight; }
            set
            {
                if (value.Equals(_outputWeight))
                    return;
                _outputWeight = value;
                RaisePropertyChanged("OutputWeight");
            }
        }

        public override void UpdateContainer()
        {
            if (Container == null)
                Container = new ProcessingItem(Id);
            Container.Number = Number;
            Container.InputNomenclatureId = InputNomenclature.Id;
            Container.InputWeight = InputWeight;
            Container.OutputNomenclatureId = OutputNomenclature.Id;
            Container.OutputWeight = OutputWeight;
        }
    }
}