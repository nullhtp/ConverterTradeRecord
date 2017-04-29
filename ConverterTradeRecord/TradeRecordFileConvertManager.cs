using ConverterTradeRecord.Entities;
using ConverterTradeRecord.Interfaces;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace ConverterTradeRecord
{
    /// <summary>
    /// Класс менеджер для создания задач и управления ими, работает только с TradeRecord
    /// </summary>
    public class TradeRecordFileConvertManager : IConverterManager<TradeRecord>
    {
        //TODO: Сделать класс универсальным, для всех структур 

        private List<TaskConverter<TradeRecord>> _tasksConverters;

        public TradeRecordFileConvertManager()
        {
            _tasksConverters = new List<TaskConverter<TradeRecord>>();
        }

        public void Add(string srcPath, string dstPath)
        {
            Add(new TradeRecordConverter(
                new CsvTradeRecordSerializator(File.OpenWrite(dstPath)),
                new BinaryTradeRecordDeserializator(File.OpenRead(srcPath))
                ));

        }

        
        /// <summary>
        /// Добавление конвертера в коллекцию и его запуск
        /// </summary>
        /// <param name="srcPath"></param>
        /// <param name="dstPath"></param>
        public void AddAndRun(string srcPath, string dstPath)
        {
            //TODO: Заменить на фабричный метод
            var converter = new TradeRecordConverter(
                new CsvTradeRecordSerializator(File.OpenWrite(dstPath)),
                new BinaryTradeRecordDeserializator(File.OpenRead(srcPath))
                );

            _tasksConverters.Add(new TaskConverter<TradeRecord>
            {
                Converter = converter,
                Task = Task.Run(() =>
                {
                    converter.Convert();
                })
            });

        }

        /// <summary>
        /// Добавление конвертера в коллекцию без его запуска
        /// </summary>
        /// <param name="converter"></param>
        private void Add(AbstractConverter<TradeRecord> converter)
        {
            _tasksConverters.Add(new TaskConverter<TradeRecord> { Converter = converter });
        }

        public float GetProgress()
        {
            return _tasksConverters.Average(tc => tc.Converter.GetProgressProcent());
        }


        public float GetProgress(int id)
        {
            float progress = 0;
            var taskConverter = Get(id);
            if (taskConverter != null)
            {
                progress = taskConverter.Converter.GetProgressProcent();
            }
            return progress;
        }

        public void RunAllAsync()
        {
            _tasksConverters.ForEach(tc =>
            {
                tc.Task = Task.Run(() =>
                {
                    tc.Converter.Convert();
                });
            });
        }

        public void RunAsync(AbstractConverter<TradeRecord> converter)
        {
            var taskConverter = Get(converter);
            if (taskConverter != null)
            {
                taskConverter.Task = Task.Run(() =>
                {
                    taskConverter.Converter.Convert();
                });
            }
        }

        private TaskConverter<TradeRecord> Get(int id)
        {
            return _tasksConverters.FirstOrDefault(c => c.Task.Id == id);
        }

        private TaskConverter<TradeRecord> Get(AbstractConverter<TradeRecord> converter)
        {
            return _tasksConverters.FirstOrDefault(c => c.Converter == converter);
        }

        public void DisplayAllId()
        {
            foreach (var taskConvert in _tasksConverters)
            {
                Console.WriteLine($"Task Id: {taskConvert.Task.Id}");
            }
        }

        void IConverterManager<TradeRecord>.Add(AbstractConverter<TradeRecord> converter)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Класс для сопоставления конвертера и задачи
        /// </summary>
        /// <typeparam name="TradeRecord"></typeparam>
        class TaskConverter<TradeRecord>
        {
            public Task Task { get; set; }
            public AbstractConverter<TradeRecord> Converter { get; set; }
        }
    }
}
